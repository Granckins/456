"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var RevsInfo = /** @class */ (function () {
    function RevsInfo() {
    }
    return RevsInfo;
}());
var Warehouse = /** @class */ (function () {
    function Warehouse() {
    }
    return Warehouse;
}());
exports.Warehouse = Warehouse;
var EventCouch = /** @class */ (function () {
    function EventCouch() {
        this._revs_info = new Array();
        this.wars = new Array();
        this.expand = false;
    }
    return EventCouch;
}());
exports.EventCouch = EventCouch;
var RowCouch = /** @class */ (function () {
    function RowCouch() {
    }
    return RowCouch;
}());
exports.RowCouch = RowCouch;
var CouchRequestClass = /** @class */ (function () {
    function CouchRequestClass() {
        this.rows = new Array();
        this.wars = new Array();
    }
    return CouchRequestClass;
}());
//# sourceMappingURL=home.model.js.map