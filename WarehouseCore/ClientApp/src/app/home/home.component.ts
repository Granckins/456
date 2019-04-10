import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { DataSetService } from '../Services/home.service';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CouchRequest, EventCouch } from '../Models/home.model';  
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { ElementRef} from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatChipInputEvent, MatAutocomplete } from '@angular/material';

export interface Fruit {
  name: string;
  value: string;
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
export class HomeComponent implements AfterViewInit {
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



  constructor(public dataService: DataSetService) {
    this.isBalanced();
    this.filteredFruits = this.fruitCtrl.valueChanges.pipe(
      startWith(null),
      map((fruit: string | null) => fruit ? this._filter(fruit) : this.allFruits.slice()));
  }

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
  condition(fruitname: string): boolean {
    if (fruitname == '(' || fruitname == ')' || fruitname == 'И' || fruitname == 'ИЛИ') {
      this.isBalanced();
      return false;
    }
    else return true;
  }
  visible = true;
  selectable = true;
  removable = true;
  Balanced = true;
  addOnBlur = false;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  fruitCtrl = new FormControl();
  filteredFruits: Observable<string[]>;
  fruits: Fruit[] = [];
  allFruits: string[] = ['Все поля', 'Наименование', 'номер упаковки', '(', ')', 'И', 'ИЛИ'];

  @ViewChild('fruitInput') fruitInput: ElementRef<HTMLInputElement>;



  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;




    // Add our fruit
    if ((value || '').trim() && this.allFruits.indexOf(value) > -1) {
      this.fruits.push({ name: value.trim(), value: '' });
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }

    this.fruitCtrl.setValue(null);
  }

  remove(fruit: Fruit): void {
    const index = this.fruits.indexOf(fruit);
    if (index >= 0) {
      this.fruits.splice(index, 1);
    }
    this.isBalanced();
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.fruits.push({ name: event.option.viewValue, value: '' });
    this.fruitInput.nativeElement.value = '';
    this.fruitCtrl.setValue(null);
    this.fruitInput.nativeElement.blur();
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.allFruits.filter(option => option.toLowerCase().includes(filterValue));
  }

  isBalanced(): boolean {
    var str = "";
    this.fruits.forEach(function (value) {
      if (value.name == '(' || value.name == ')')
        str += value.name;
      });
 
   
    let stack = [];
    let map = {
      '(': ')',
      '[': ']',
      '{': '}'
    }
    if (str === "") { this.Balanced = true; return true;}
    for (let i = 0; i < str.length; i++) {

      // If character is an opening brace add it to a stack
      if (str[i] === '(' || str[i] === '{' || str[i] === '[') {
        stack.push(str[i]);
      }
      //  If that character is a closing brace, pop from the stack, which will also reduce the length of the stack each time a closing bracket is encountered.
      else {
        let last = stack.pop();

        //If the popped element from the stack, which is the last opening brace doesn’t match the corresponding closing brace in the map, then return false
        if (str[i] !== map[last]) { this.Balanced = false; return false;};
      }
    }
    // By the completion of the for loop after checking all the brackets of the str, at the end, if the stack is not empty then fail
    if (stack.length !== 0) { this.Balanced = false; return false; };

    this.Balanced = true; return true;
  } 
}
