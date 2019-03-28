import { Component, OnInit, ViewChild} from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { DataSetService } from '../Services/home.service';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

import { CouchRequest, EventCouch } from '../Models/home.model'; 
import { MatTable } from '@angular/material';
export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {

  @ViewChild(MatTable) table: MatTable<any>;
 
  dataSource = new MatTableDataSource<EventCouch>();
  dataSource2 = new MatTableDataSource<EventCouch>();
  ELEMENT_DATA: EventCouch[] = [
    
  ];

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
  displayedColumns: string[] = ['Nomer_upakovki', 'Naimenovanie_izdeliya'/*, 'Zavodskoj_nomer', 'Kolichestvo','Mestonahozhdenie_na_sklade'*/ ];
 

  constructor(public dataService: DataSetService) { }

  ngOnInit() {
    this.RenderDataTable();
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
