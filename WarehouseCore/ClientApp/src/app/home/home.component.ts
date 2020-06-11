import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { DataSetService } from '../Services/home.service'; 
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CouchRequest, EventCouch, Warehouse } from '../Models/home.model';  
import { merge, Observable, of as observableOf, BehaviorSubject } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { ElementRef} from '@angular/core';
import { FormControl } from '@angular/forms';
 


import { Sort } from '@angular/material/sort';

export interface Dessert {
  calories: number;
  carbs: number;
  fat: number;
  name: string;
  protein: number;
}

export interface Fruit {
  name: string;
  value: string;
  id:number;
}
 
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
 
export class HomeComponent {
    desserts: Dessert[] = [
      { name: 'Frozen yogurt', calories: 159, fat: 6, carbs: 24, protein: 4 },
      { name: 'Ice cream sandwich', calories: 237, fat: 9, carbs: 37, protein: 4 },
      { name: 'Eclair', calories: 262, fat: 16, carbs: 24, protein: 6 },
      { name: 'Cupcake', calories: 305, fat: 4, carbs: 67, protein: 4 },
      { name: 'Gingerbread', calories: 356, fat: 16, carbs: 49, protein: 4 },
    ];

  sortedData: Dessert[];

  constructor() {
    this.sortedData = this.desserts.slice();
  }

  sortData(sort: Sort) {
    const data = this.desserts.slice();
    if (!sort.active || sort.direction === '') {
      this.sortedData = data;
      return;
    }

    this.sortedData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'name': return compare(a.name, b.name, isAsc);
        case 'calories': return compare(a.calories, b.calories, isAsc);
        case 'fat': return compare(a.fat, b.fat, isAsc);
        case 'carbs': return compare(a.carbs, b.carbs, isAsc);
        case 'protein': return compare(a.protein, b.protein, isAsc);
        default: return 0;
      }
    });
  }
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
