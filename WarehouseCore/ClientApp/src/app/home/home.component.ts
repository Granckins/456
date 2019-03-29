import { Component, OnInit, ViewChild} from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { DataSetService } from '../Services/home.service';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

import { animate, state, style, transition, trigger } from '@angular/animations';
import { CouchRequest, EventCouch } from '../Models/home.model'; 
import { MatTable } from '@angular/material';
  

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
export class HomeComponent implements OnInit {

  @ViewChild(MatTable) table: MatTable<any>;
 
  dataSource = new MatTableDataSource<EventCouch>();
  dataSource2 = new MatTableDataSource<EventCouch>();
  ELEMENT_DATA: EventCouch[] = [
    
  ];
  expandedElement: EventCouch | null;
  expandedRow: number;

  isExpansionDetailRow = (i: number, row: Object) => row.hasOwnProperty('detailRow');
 
  ELEMENT_DATA2: EventCouch[] = [
    

  ];
  drop(event: CdkDragDrop<string[]>) {
     
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }
    this.dataSource = new MatTableDataSource<EventCouch>(this.ELEMENT_DATA);
    this.dataSource2 = new MatTableDataSource<EventCouch>(this.ELEMENT_DATA2);
  }
  displayedColumns: string[] = ['Nomer_upakovki', 'Naimenovanie_izdeliya' ,'Zavodskoj_nomer', 'Kolichestvo','Mestonahozhdenie_na_sklade'
    , 'Oboznachenie', 'Sistema', 'Otvetstvennyj', 'Prinadlezhnost',      'Data_priyoma', 'Otkuda', 'Data_vydachi', 'Kuda', 'Nomer_plomby',
      'Stoimost', 'Ves_brutto', 'Ves_netto', 'Dlina', 'Shirina',     'Vysota', 'Primechanie', 'Dobavil','Data_ismenen'];
 

  constructor(public dataService: DataSetService) { }

  ngOnInit() {
    this.RenderDataTable();
  }
  expandRow(e: EventCouch) {

    e.expand = e.expand ? false : true;
  }
  RenderDataTable() {
    this.dataService.getUser()
      .subscribe(
        res => { 
          this.dataSource.data= res.rows;
          this.ELEMENT_DATA = res.rows;
        },
        error => {
          console.log('There was an error while retrieving Posts !!!' + error);
        });
  }

}  
