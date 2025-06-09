define(["amplify", "jquery"], function(obj) {  
    
    var getValue = function(key) {
        return amplify.store(key);
    }
    
    var setValue = function(key, value) {
        amplify.store(key, value);
    }
    
    var getSession = function(key) {
        return amplify.store.sessionStorage(key);
    }
    
    var setSession = function(key, value) {
        amplify.store.sessionStorage(key, value);
    }
    
    var getAll = function() {
        return amplify.store();
    }
    
    var remove = function(key) {
        amplify.store(key, null);
    }
    
    //Return functions
    return {
        getValue: getValue,
        setValue: setValue,
        getSession: getValue,
        setSession: setValue,
        getAll: getAll,
        remove: remove
    }
});