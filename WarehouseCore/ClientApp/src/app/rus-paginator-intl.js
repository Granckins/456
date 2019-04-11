"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var material_1 = require("@angular/material");
var rusRangeLabel = function (page, pageSize, length) {
    if (length == 0 || pageSize == 0) {
        return "0 \u043D\u0430 " + length;
    }
    length = Math.max(length, 0);
    var startIndex = page * pageSize;
    // If the start index exceeds the list length, do not try and fix the end index to the end.
    var endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
    return startIndex + 1 + " - " + endIndex + " \u043D\u0430 " + length;
};
function getRusPaginatorIntl() {
    var paginatorIntl = new material_1.MatPaginatorIntl();
    paginatorIntl.itemsPerPageLabel = 'Строк на странице:';
    paginatorIntl.nextPageLabel = 'Предыдущая страница';
    paginatorIntl.previousPageLabel = 'Следующая страница';
    paginatorIntl.getRangeLabel = rusRangeLabel;
    return paginatorIntl;
}
exports.getRusPaginatorIntl = getRusPaginatorIntl;
//# sourceMappingURL=rus-paginator-intl.js.map