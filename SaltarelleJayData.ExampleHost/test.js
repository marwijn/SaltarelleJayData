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
        var database = new $SaltarelleJayData_Example_Database();
    };

    ////////////////////////////////////////////////////////////////////////////////
    // SaltarelleJayData.Example.MyEntity
    var $SaltarelleJayData_Example_MyEntity = function () {
        this.BInt = 0;
        JayDataApi.Entity.call(this);
    };
    $SaltarelleJayData_Example_MyEntity.$jayDataConstructor = $data.Entity.extend('SaltarelleJayData.Example.MyEntity', { BInt: { type: 'int' } });
    $SaltarelleJayData_Example_MyEntity.__typeName = 'SaltarelleJayData.Example.MyEntity';
    global.SaltarelleJayData.Example.MyEntity = $SaltarelleJayData_Example_MyEntity;

    ////////////////////////////////////////////////////////////////////////////////
    // SaltarelleJayData.Example.Database
    var $SaltarelleJayData_Example_Database = function () {
        this.TheBs = null;
        JayDataApi.EntityContext.call(this, 'TEST');
    };
    $SaltarelleJayData_Example_Database.$jayDataConstructor = $data.EntityContext.extend('SaltarelleJayData.Example.Database', { TheBs: { type: '$data.EntitySet', elementType: 'SaltarelleJayData.Example.MyEntity' } });
    $SaltarelleJayData_Example_Database.__typeName = 'SaltarelleJayData.Example.Database';
    global.SaltarelleJayData.Example.Database = $SaltarelleJayData_Example_Database;
    ss.initClass($SaltarelleJayData_Example_$Program, $asm, {});
    ss.initClass($SaltarelleJayData_Example_Database, $asm, {}, JayDataApi.EntityContext);
    ss.initClass($SaltarelleJayData_Example_MyEntity, $asm, {}, JayDataApi.Entity);
    $SaltarelleJayData_Example_$Program.$main();
})();
