import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { DataSetService } from '../Services/home.service';

import { CouchRequest, EventCouch } from '../Models/home.model';
import { MatSort } from '@angular/material'; 

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {

  
  displayedColumns: string[] = ['Nomer_upakovki', 'Naimenovanie_izdeliya', 'Zavodskoj_nomer', 'Kolichestvo','Mestonahozhdenie_na_sklade' ];
  dataSource = new MatTableDataSource<EventCouch>();

  constructor(public dataService: DataSetService) { }

  ngOnInit() {
    this.RenderDataTable();
  }

  RenderDataTable() {
    this.dataService.getUser()
      .subscribe(
        res => { 
          this.dataSource.data= res.rows;
         
        },
        error => {
          console.log('There was an error while retrieving Posts !!!' + error);
        });
  }

}  
