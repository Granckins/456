import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { DataSetService } from '../Services/home.service';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CouchRequest, EventCouch } from '../Models/home.model';  
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class HomeComponent implements AfterViewInit  {
  displayedColumns: string[] = ['Nomer_upakovki', 'Naimenovanie_izdeliya', 'Zavodskoj_nomer', 'Kolichestvo', 'Mestonahozhdenie_na_sklade'
    , 'Oboznachenie', 'Sistema', 'Otvetstvennyj', 'Prinadlezhnost', 'Data_priyoma', 'Otkuda', 'Data_vydachi', 'Kuda', 'Nomer_plomby',
    'Stoimost', 'Ves_brutto', 'Ves_netto', 'Dlina', 'Shirina', 'Vysota', 'Primechanie', 'Dobavil', 'Data_ismenen'];
  exampleDatabase: DataSetService | null;
  data: EventCouch[] = [];

  resultsLength = 0;
  isLoadingResults = true;
  isRateLimitReached = false;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
   

  expandedElement: EventCouch | null;
  expandedRow: number;

  isExpansionDetailRow = (i: number, row: Object) => row.hasOwnProperty('detailRow');
 
 

  constructor(public dataService: DataSetService) { }

  ngAfterViewInit() {
    

    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.dataService.getUser(this.sort.active, this.sort.direction, this.paginator.pageIndex);
     }),
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoadingResults = false;
          this.isRateLimitReached = false;
          this.resultsLength = data.total_rows;

          return data.rows;
        }),
        catchError(() => {
          this.isLoadingResults = false;
          // Catch if the GitHub API has reached its rate limit. Return empty data.
          this.isRateLimitReached = true;
          return observableOf([]);
        })
      ).subscribe(data => this.data = data);
  }
 
  expandRow(e: EventCouch) {

    e.expand = e.expand ? false : true;
  }
 
  /** Builds and returns a new User. */
 
 
}  

