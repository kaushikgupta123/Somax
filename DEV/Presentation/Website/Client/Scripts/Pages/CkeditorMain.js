//#region ckeditor 5
let theEditor = "";
function LoadCkEditor(equtxtcomments) {
    $(".toolbar-container").html('');
    ClearEditor();
    DecoupledEditor
        .create(document.querySelector('#' + equtxtcomments), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MentionCustomization],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            },
            mention: {
                feeds: [
                    {
                        marker: '@',
                        feed: getFeedItems,
                    }
                ]
            }

        })
        .then(editor => {
            //editor.destroy();
            const toolbarContainer = document.querySelector('main .toolbar-container');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            theEditor = editor;
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {

            });
        })
        .catch(err => {
            console.error(err.stack);
        });
}

function LoadCkEditorEdit(equtxtcomments, data) {
    $(".toolbar-containerEdit").html('');
    ClearEditorEdit();
    DecoupledEditor
        .create(document.querySelector('#' + equtxtcomments), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MentionCustomization],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            },
            mention: {
                feeds: [
                    {
                        marker: '@',
                        feed: getFeedItems,
                    }
                ]

            }
        })
        .then(editor => {
            //editor.destroy();
            var getParseHtml = GetParseHtml(data);
            editor.setData(getParseHtml);
            const toolbarContainer = document.querySelector('main .toolbar-containerEdit');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            theEditor = editor;
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {

            });
        })
        .catch(err => {
            console.error(err.stack);
        });
}

function MentionCustomization(editor) {
    // The upcast converter will convert <a class="mention" href="" data-user-id="">
    // elements to the model 'mention' attribute.
    editor.conversion.for('upcast').elementToAttribute({
        view: {
            name: 'span',
            key: 'data-mention',
            classes: 'mention',
            attributes: {
                // href: ,
                'data-user-id': true
            }
        },
        model: {
            key: 'mention',
            value: viewItem => {
                // The mention feature expects that the mention attribute value
                // in the model is a plain object with a set of additional attributes.
                // In order to create a proper object, use the toMentionAttribute helper method:
                const mentionAttribute = editor.plugins.get('Mention').toMentionAttribute(viewItem, {
                    // Add any other properties that you need.
                    //link: viewItem.getAttribute('href'),
                    userId: viewItem.getAttribute('data-user-id')
                });

                return mentionAttribute;
            }
        },
        converterPriority: 'high'
    });

    // Downcast the model 'mention' text attribute to a view <a> element.
    editor.conversion.for('downcast').attributeToElement({
        model: 'mention',
        view: (modelAttributeValue, { writer }) => {
            // Do not convert empty attributes (lack of value means no mention).
            if (!modelAttributeValue) {
                return;
            }

            return writer.createAttributeElement('span', {
                class: 'mention',
                'data-mention': modelAttributeValue.id,
                'data-user-id': modelAttributeValue.name,
            }, {
                // Make mention attribute to be wrapped by other attribute elements.
                priority: 20,
                // Prevent merging mentions together.
                id: modelAttributeValue.uid
            });
        },
        converterPriority: 'high'
    });
}


var items = [];
function getFeedItems(queryText) {
    // As an example of an asynchronous action, return a promise
    // that resolves after a 100ms timeout.
    // This can be a server request or any sort of delayed action.
    return new Promise(resolve => {
        setTimeout(() => {
            $.ajax({
                url: '/Base/GetMentionList',
                type: 'GET',
                data: { searchText: queryText },
                success: function (data) {
                    var i;
                    items = [];
                    for (i = 0; i < data.length; i++) {
                        items.push(
                            {
                                id: '@' + data[i].name,
                                name: data[i].id
                            });
                    }
                },
                complete: function () {
                    //CloseLoader();
                }
            });
            items = uniqueArray(items);
            const itemsToDisplay = items
                // Filter out the full list of all items to only those matching the query text.
                .filter(isItemMatching)
                // Return 10 items max - needed for generic queries when the list may contain hundreds of elements.
                .slice(0, 10);

            resolve(itemsToDisplay);
        }, 100);
    });

    // Filtering function - it uses the `name` and `username` properties of an item to find a match.
    function isItemMatching(item) {
        // Make the search case-insensitive.
        const searchString = queryText.toLowerCase();

        // Include an item in the search results if the name or username includes the current user input.
        return (
            item.name.toLowerCase().includes(searchString) ||
            item.id.toLowerCase().includes(searchString)
        );
    }
}

function uniqueArray(myArray) {
    var newArray = [];
    $.each(myArray, function (key, value) {
        var exists = false;
        $.each(newArray, function (k, val2) {
            if (value.id == val2.id) { exists = true };
        });
        if (exists == false && value.id != "") { newArray.push(value); }
    });
    return newArray;
}
function getDataFromTheEditor() {
    return theEditor.getData();
}
function ClearEditor() {
    if (theEditor != "") {
        theEditor.setData('');
        $(".toolbar-container").html('');
        theEditor.destroy(true);
        theEditor = "";
    }
}
function ClearEditorEdit() {
    if (theEditor != "") {
        theEditor.setData('');
        theEditor.destroy(true);
        $(".toolbar-containerEdit").html('');
        theEditor = "";
    }
}
//#endregion

function CreateEditorHTML(id) {
    return '<main style="margin-bottom:10px !important;">' +
        '<div class="document-editor ckeditorfield" >' +
        '<div class="toolbar-containerEdit"></div>' +
        '<div class="content-container form-control">' +
        '<div id="' + id + '"></div>' +
        '</div>' +
        '</div>' +
        '</main>' +
        '<button type="submit" class="btn btn-blue mobBttn btneditcomments" value="save">Save</button>' +
        '<button type="button" class="btn btn-blue mobBttn btncommandCancel">Cancel</button>';
}


//#region New Methods
let editorIntances = {};
function getDataFromEditorById(id) {
    return editorIntances[id] ? editorIntances[id].getData() : '';
}
function ClearEditorById(id) {
    if (editorIntances[id]) {
        editorIntances[id].setData('');
        //$(".mytoolbar-container").html('');
        $(document).find('#' + id).parent().siblings('.mytoolbar-container').html('');
        editorIntances[id].destroy(true);
        delete editorIntances[id];
    }
}
function LoadCkEditorById(id, succesCallback) {
    //$('.mytoolbar-container').html('');
    $(document).find('#' + id).parent().siblings('.mytoolbar-container').html('');
    ClearEditorById(id);
    DecoupledEditor
        .create(document.querySelector('#' + id), {
            toolbar: ['heading', '|', 'bold', 'italic', 'alignment', 'link', 'numberedList', 'bulletedList', '|', 'fontFamily', 'fontSize', 'fontColor', 'fontBackgroundColor', '|', 'removeFormat'],
            extraPlugins: [MentionCustomization],
            mediaEmbed: { previewsInData: true },
            fontSize: {
                options: [8, 9, 10, 11, 12, 13, 14, 16, 18, 24, 30, 36, 48, 60, 72, 96]
            },
            mention: {
                feeds: [
                    {
                        marker: '@',
                        feed: getFeedItems,
                    }
                ]
            }
        })
        .then(editor => {
            //var toolbarContainer = document.querySelector('main .mytoolbar-container');
            var toolbarContainer = $(document).find('#' + id).parent().siblings('.mytoolbar-container');
            toolbarContainer.prepend(editor.ui.view.toolbar.element);
            editor.execute('listStyle', { type: 'decimal' });
            editor.model.document.on('change:data', () => {
            });
            editorIntances[id] = editor;
            if ($.isFunction(succesCallback)) {
                succesCallback(editor);
            }
        })
        .catch(err => {
            console.error(err.stack);
        });
}
function SetDataById(id, content) {
    if (editorIntances[id]) {
        var getTexttoHtml = textToHTML(content);
        editorIntances[id].setData(getTexttoHtml);
    }
}
//#endregion

//#region Get parse html for comment
function GetParseHtml(data) {
    var Fullhtml = "";
    for (var i = 0; i < data.length; i++) {
        var inntext = data[i].innerText;
        if (typeof (inntext) != "undefined") {
            if (inntext.indexOf('</html>') > -1) {
                var endodedhtml = HtmlEncode(inntext);
                var outHtml = data[i].outerHTML;
                outHtml = outHtml.replace(endodedhtml, inntext);
                if (outHtml.indexOf('<html><body>') > 1) {
                    outHtml = outHtml.replace('<html><body>', '');
                    outHtml = outHtml.replace('</body></html>', '');
                }
                Fullhtml += outHtml;
            }
            else {
                Fullhtml += data[i].outerHTML;
            }
        }
    }
    return Fullhtml;
}
function HtmlEncode(s) {
    var el = document.createElement("div");
    el.innerText = el.textContent = s;
    s = el.innerHTML;
    return s;
}

//return only html
function support(str) {
    if (!window.DOMParser) return false;
    var parser = new DOMParser();
    try {
        parser.parseFromString(str, 'text/html');
    } catch (err) {
        return false;
    }
    return true;
}

var textToHTML = function (str) {
    // check for DOMParser support
    str = checkTags(str);
    if (support(str)) {
        var parser = new DOMParser();
        var doc = parser.parseFromString(str, 'text/html');
        return doc.body.innerHTML;
    }

    // Otherwise, create div and append HTML
    var dom = document.createElement('div');
    dom.innerHTML = str;
    return dom;

};
function checkTags(str) {
    var DOMHolderArray = new Array();
    var tagsArray = new Array();
    var lines = str.split('\n');
    for (var x = 0; x < lines.length; x++) {
        var tagsArray = lines[x].match(/<(\/{1})?\w+((\s+\w+(\s*=\s*(?:".*?"|'.*?'|[^'">\s]+))?)+\s*|\s*)>/g);
        if (tagsArray) {
            for (var i = 0; i < tagsArray.length; i++) {
                if (tagsArray[i].indexOf('</') >= 0) {
                    elementToPop = tagsArray[i].substr(2, tagsArray[i].length - 3);
                    elementToPop = elementToPop.replace(/ /g, '');
                    for (var j = DOMHolderArray.length - 1; j >= 0; j--) {
                        if (DOMHolderArray[j].element == elementToPop) {
                            DOMHolderArray.splice(j, 1);
                            if (elementToPop != 'html') {
                                break;
                            }
                        }
                    }
                } else {
                    var tag = new Object();
                    tag.full = tagsArray[i];
                    tag.line = x + 1;
                    if (tag.full.indexOf(' ') > 0) {
                        tag.element = tag.full.substr(1, tag.full.indexOf(' ') - 1);
                    } else {
                        tag.element = tag.full.substr(1, tag.full.length - 2);
                    }
                    var selfClosingTags = new Array('area', 'base', 'br', 'col', 'command', 'embed', 'hr', 'img', 'input', 'keygen', 'link', 'meta', 'param', 'source', 'track', 'wbr');
                    var isSelfClosing = false;
                    for (var y = 0; y < selfClosingTags.length; y++) {
                        if (selfClosingTags[y].localeCompare(tag.element) == 0) {
                            isSelfClosing = true;
                        }
                    }
                    if (isSelfClosing == false) {
                        DOMHolderArray.push(tag);
                    }
                }

            }
        }
    }
    var uniqueArr = getUniqueValues(DOMHolderArray, 'element')

    if (uniqueArr.length > 0) {
        for (var i = 0; i < uniqueArr.length; i++) {
            if (str.indexOf('</' + uniqueArr[i]) > -1) {
                str = str.replaceAll('</' + uniqueArr[i], '</' + uniqueArr[i] + '>');
                str = str.replaceAll('</' + uniqueArr[i] + '>>', '</' + uniqueArr[i] + '>');
            }
        }
    }
    return str;

}

function getUniqueValues(array, key) {
    var result = [];
    array.forEach(function (item) {
        if (item.hasOwnProperty(key)) {
            result.push(item[key]);
        }
    });
    var unique = result.filter(function (itm, i, result) {
        return i == result.indexOf(itm);
    });
    return unique;
}
//#endregion
