﻿(function() {
	'use strict';
	var $asm = {};
	global.JayDataApi = global.JayDataApi || {};
	ss.initAssembly($asm, 'Saltarelle.JayData');
	////////////////////////////////////////////////////////////////////////////////
	// JayDataApi.AsyncQueryable
	var $JayDataApi_AsyncQueryable$1 = function(T) {
		var $type = function(jayDataObject) {
			this.jayDataObject = jayDataObject;
		};
		ss.registerGenericClassInstance($type, $JayDataApi_AsyncQueryable$1, [T], {
			toList: function() {
				var jayDataTask = ss.Task.fromDoneCallback(this.jayDataObject, 'toArray');
				return jayDataTask.continueWith(function(task) {
					var list = [];
					var $t1 = ss.getEnumerator(task.getResult());
					try {
						while ($t1.moveNext()) {
							var obj = $t1.current();
							ss.add(list, $JayDataApi_Entity.$create(T).call(null, obj));
						}
					}
					finally {
						$t1.dispose();
					}
					return list;
				});
			},
			where: function(func) {
				var expression = func.toString();
				var match = (new RegExp('\\s*function\\s*\\(\\s*(.*)\\s*\\)\\s*{\\s*(.*)\\s*}.*')).exec(expression);
				expression = match[2].replace(new RegExp('\\.jayDataObject', 'g'), '');
				var changed;
				do {
					var indexOfMatch = (new RegExp('indexOf\\((.*?)\\)[\\s\\S]*?!==[\\s\\S]*?-1', 'g')).exec(expression);
					if (ss.isValue(indexOfMatch)) {
						expression = ss.replaceAllString(expression, indexOfMatch[0], 'contains(' + indexOfMatch[1] + ')');
						changed = true;
					}
					else {
						changed = false;
					}
				} while (changed);
				return new $type(this.jayDataObject.filter(new Function(match[1], expression)));
			}
		}, function() {
			return null;
		}, function() {
			return [];
		});
		return $type;
	};
	$JayDataApi_AsyncQueryable$1.__typeName = 'JayDataApi.AsyncQueryable$1';
	ss.initGenericClass($JayDataApi_AsyncQueryable$1, $asm, 1);
	global.JayDataApi.AsyncQueryable$1 = $JayDataApi_AsyncQueryable$1;
	////////////////////////////////////////////////////////////////////////////////
	// JayDataApi.ContextConfiguration
	var $JayDataApi_ContextConfiguration = function() {
		this.databaseName = null;
		this.$1$ProviderField = null;
	};
	$JayDataApi_ContextConfiguration.__typeName = 'JayDataApi.ContextConfiguration';
	global.JayDataApi.ContextConfiguration = $JayDataApi_ContextConfiguration;
	////////////////////////////////////////////////////////////////////////////////
	// JayDataApi.Entity
	var $JayDataApi_Entity = function() {
		this.jayDataObject = new this.constructor.jayDataConstructor();
	};
	$JayDataApi_Entity.__typeName = 'JayDataApi.Entity';
	$JayDataApi_Entity.$create = function(T) {
		return function(jayDataObject) {
			var entity = ss.createInstance(T);
			entity.jayDataObject = jayDataObject;
			return entity;
		};
	};
	global.JayDataApi.Entity = $JayDataApi_Entity;
	////////////////////////////////////////////////////////////////////////////////
	// JayDataApi.EntityContext
	var $JayDataApi_EntityContext = function(database) {
		this.jayDataObject = new this.constructor.jayDataConstructor(database);
	};
	$JayDataApi_EntityContext.__typeName = 'JayDataApi.EntityContext';
	global.JayDataApi.EntityContext = $JayDataApi_EntityContext;
	////////////////////////////////////////////////////////////////////////////////
	// JayDataApi.EntitySet
	var $JayDataApi_EntitySet$1 = function(T) {
		var $type = function(jayDataObject) {
			ss.makeGenericType($JayDataApi_AsyncQueryable$1, [T]).call(this, jayDataObject);
		};
		ss.registerGenericClassInstance($type, $JayDataApi_EntitySet$1, [T], {}, function() {
			return ss.makeGenericType($JayDataApi_AsyncQueryable$1, [T]);
		}, function() {
			return [];
		});
		return $type;
	};
	$JayDataApi_EntitySet$1.__typeName = 'JayDataApi.EntitySet$1';
	ss.initGenericClass($JayDataApi_EntitySet$1, $asm, 1);
	global.JayDataApi.EntitySet$1 = $JayDataApi_EntitySet$1;
	ss.initClass($JayDataApi_ContextConfiguration, $asm, {
		get_provider: function() {
			return this.$1$ProviderField;
		},
		set_provider: function(value) {
			this.$1$ProviderField = value;
		}
	});
	ss.initClass($JayDataApi_Entity, $asm, {
		toString: function() {
			return JSON.stringify(this.jayDataObject.toJSON());
		}
	});
	ss.initClass($JayDataApi_EntityContext, $asm, {
		ready: function() {
			return ss.cast(ss.Task.fromDoneCallback(this.jayDataObject, 'onReady'), ss.Task);
		},
		saveChanges: function() {
			//return Task.FromPromise(JayDataObject.saveChanges());
			return ss.cast(ss.Task.fromDoneCallback(this.jayDataObject, 'saveChanges'), ss.Task);
		}
	});
})();
