(function() {
	'use strict';
	var $asm = {};
	global.SaltarelleJayData = global.SaltarelleJayData || {};
	global.SaltarelleJayData.Example = global.SaltarelleJayData.Example || {};
	ss.initAssembly($asm, 'SaltarelleJayData.Example');
	////////////////////////////////////////////////////////////////////////////////
	// SaltarelleJayData.Example.Program
	var $SaltarelleJayData_Example_$Program = function() {
	};
	$SaltarelleJayData_Example_$Program.__typeName = 'SaltarelleJayData.Example.$Program';
	$SaltarelleJayData_Example_$Program.$main = function() {
		$($SaltarelleJayData_Example_$Program.$run);
	};
	$SaltarelleJayData_Example_$Program.$run = function() {
		var $state = 0, entity, database, $t1, $t2, $t3, entities;
		var $sm = function() {
			$sm1:
			for (;;) {
				switch ($state) {
					case 0: {
						$state = -1;
						entity = new $SaltarelleJayData_Example_MyEntity();
						database = new $SaltarelleJayData_Example_Database();
						$t1 = database.ready();
						$state = 1;
						$t1.continueWith($sm);
						return;
					}
					case 1: {
						$state = -1;
						$t1.getResult();
						database.TheBs.jayDataObject.add(entity.jayDataObject);
						$t2 = database.saveChanges();
						$state = 2;
						$t2.continueWith($sm);
						return;
					}
					case 2: {
						$state = -1;
						$t2.getResult();
						$t3 = database.TheBs.toList();
						$state = 3;
						$t3.continueWith($sm);
						return;
					}
					case 3: {
						$state = -1;
						entities = $t3.getResult();
						$('#content').html(ss.count(entities).toString());
						$state = -1;
						break $sm1;
					}
					default: {
						break $sm1;
					}
				}
			}
		};
		$sm();
	};
	////////////////////////////////////////////////////////////////////////////////
	// SaltarelleJayData.Example.MyEntity
	var $SaltarelleJayData_Example_MyEntity = function() {
		JayDataApi.Entity.call(this);
	};
	$SaltarelleJayData_Example_MyEntity.jayDataConstructor = $data.Entity.extend('SaltarelleJayData.Example.MyEntity', { BInt: { type: 'int', key: true, computed: true } });
	$SaltarelleJayData_Example_MyEntity.__typeName = 'SaltarelleJayData.Example.MyEntity';
	global.SaltarelleJayData.Example.MyEntity = $SaltarelleJayData_Example_MyEntity;
	////////////////////////////////////////////////////////////////////////////////
	// SaltarelleJayData.Example.Database
	var $SaltarelleJayData_Example_Database = function() {
		JayDataApi.EntityContext.call(this, 'TEST');
		this.TheBs = new (ss.makeGenericType(JayDataApi.EntitySet$1, [$SaltarelleJayData_Example_MyEntity]))(this.jayDataObject.TheBs);
	};
	$SaltarelleJayData_Example_Database.jayDataConstructor = $data.EntityContext.extend('SaltarelleJayData.Example.Database', { TheBs: { type: '$data.EntitySet', elementType: 'SaltarelleJayData.Example.MyEntity' } });
	$SaltarelleJayData_Example_Database.__typeName = 'SaltarelleJayData.Example.Database';
	global.SaltarelleJayData.Example.Database = $SaltarelleJayData_Example_Database;
	ss.initClass($SaltarelleJayData_Example_$Program, $asm, {});
	ss.initClass($SaltarelleJayData_Example_Database, $asm, {}, JayDataApi.EntityContext);
	ss.initClass($SaltarelleJayData_Example_MyEntity, $asm, {}, JayDataApi.Entity);
	$SaltarelleJayData_Example_$Program.$main();
})();
