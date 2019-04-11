import { MatPaginatorIntl } from '@angular/material';

const rusRangeLabel = (page: number, pageSize: number, length: number) => {
  if (length == 0 || pageSize == 0) { return `0 на ${length}`; }

  length = Math.max(length, 0);

  const startIndex = page * pageSize;

  // If the start index exceeds the list length, do not try and fix the end index to the end.
  const endIndex = startIndex < length ?
    Math.min(startIndex + pageSize, length) :
    startIndex + pageSize;

  return `${startIndex + 1} - ${endIndex} на ${length}`;
}


export function getRusPaginatorIntl() {
  const paginatorIntl = new MatPaginatorIntl();

  paginatorIntl.itemsPerPageLabel = 'Строк на странице:';
  paginatorIntl.nextPageLabel = 'Предыдущая страница';
  paginatorIntl.previousPageLabel = 'Следующая страница';
  paginatorIntl.getRangeLabel = rusRangeLabel;

  return paginatorIntl;
}
