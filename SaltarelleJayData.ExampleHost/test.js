(function () {
    'use strict';
    var $asm = {};
    global.SaltarelleJayData = global.SaltarelleJayData || {};
    global.SaltarelleJayData.Example = global.SaltarelleJayData.Example || {};
    ss.initAssembly($asm, 'SaltarelleJayData.Example');
    ////////////////////////////////////////////////////////////////////////////////
    // SaltarelleJayData.Example.Program
    var $SaltarelleJayData_Example_$Program = function () {
    };
    $SaltarelleJayData_Example_$Program.__typeName = 'SaltarelleJayData.Example.$Program';
    $SaltarelleJayData_Example_$Program.$main = function () {
        var entity = new $SaltarelleJayData_Example_MyEntity();
        entity.$jayDataObject.BInt = 7;
        //var database = new Database();
        //var test = database.TheBs;
    };
    ////////////////////////////////////////////////////////////////////////////////
    // SaltarelleJayData.Example.MyEntity
    var $SaltarelleJayData_Example_MyEntity = function () {
        JayDataApi.Entity.call(this);
    };
    $SaltarelleJayData_Example_MyEntity.$jayDataConstructor = $data.Entity.extend('SaltarelleJayData.Example.MyEntity', {});
    $SaltarelleJayData_Example_MyEntity.__typeName = 'SaltarelleJayData.Example.MyEntity';
    global.SaltarelleJayData.Example.MyEntity = $SaltarelleJayData_Example_MyEntity;
    ss.initClass($SaltarelleJayData_Example_$Program, $asm, {});
    ss.initClass($SaltarelleJayData_Example_MyEntity, $asm, {}, JayDataApi.Entity);
    $SaltarelleJayData_Example_$Program.$main();
})();
