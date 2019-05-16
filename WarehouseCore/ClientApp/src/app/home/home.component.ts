import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { DataSetService } from '../Services/home.service';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

import { MatPaginator, MatSort, MatTableDataSource, MatPaginatorIntl  } from '@angular/material';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CouchRequest, EventCouch, Warehouse } from '../Models/home.model';  
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { ElementRef} from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatChipInputEvent, MatAutocomplete } from '@angular/material';

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
export class HomeComponent implements AfterViewInit {
  wars: Warehouse[] = [];
  displayedColumns: string[] = ['Nomer_upakovki', 'Naimenovanie_izdeliya', 'Zavodskoj_nomer', 'Kolichestvo', 'Mestonahozhdenie_na_sklade'
    , 'Oboznachenie', 'Sistema', 'Otvetstvennyj', 'Prinadlezhnost', 'Data_priyoma', 'Otkuda', 'Data_vydachi', 'Kuda', 'Nomer_plomby',
    'Stoimost', 'Ves_brutto', 'Ves_netto', 'Dlina', 'Shirina', 'Vysota', 'Primechanie', 'Dobavil', 'Data_ismenen'];
  exampleDatabase: DataSetService | null;
  data: EventCouch[] = [];

  warehouse = '';

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
          var str = this.GetFilterString();
          if (str == "---") str = "";
          return this.dataService.getUser(str, this.paginator.pageSize, this.sort.active, this.sort.direction, this.paginator.pageIndex, this.warehouse);
        }),
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoadingResults = false;
          this.isRateLimitReached = false;
          this.resultsLength = data.total_rows;
          this.wars = data.wars;
  
          return data.rows;
        }),
        catchError(() => {
          this.isLoadingResults = false;  
          this.isRateLimitReached = true;
          return observableOf([]);
        })
      ).subscribe(data => this.data = data, wars => this.wars);
  }

  expandRow(e: EventCouch) {

    e.expand = e.expand ? false : true;
  }
  condition(fruitname: string): boolean {
    if (fruitname == '(' || fruitname == ')' || fruitname == 'И' || fruitname == 'ИЛИ') {
      this.isBalanced();

      return false;
    }
    else {
     return true;
    }
  }
  visible = true;
  prevFilter = "";
  selectable = true;
  removable = true;
  Balanced = true;
 RightOperator = true;
  addOnBlur = false;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  fruitCtrl = new FormControl();
  filteredFruits: Observable<string[]>;
  fruits: Fruit[] = [];
  Wars: Warehouse[] = [];
  allFruits: string[] = ['(', ')', 'И', 'ИЛИ', 'Все поля', 'Номер упаковки', 'Наименование изделия',
    'Заводской номер', 'Обозначение', 'Система', 'Принадлежность', 'Ответственный', 'Местонахождение',
    'Откуда', 'Куда', 'Система', 'Примечание', 'Добавил'   ];
  


  @ViewChild('fruitInput') fruitInput: ElementRef<HTMLInputElement>;



  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;




    // Add our fruit
    if ((value || '').trim() && this.allFruits.indexOf(value) > -1) {
      this.fruits.push({ name: value.trim(), value: '', id: this.fruits.length });
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }
    this.isRightOperator();
    this.isBalanced();
    if (this.isOkChip()) {
      this.FilterString();
    }
    this.fruitCtrl.setValue(null);
  }

  remove(fruit: Fruit): void {
    const index = this.fruits.indexOf(fruit);
    if (index >= 0) {
      this.fruits.splice(index, 1);
    }

    this.paginator.pageIndex = 0;
    this.isRightOperator();
    this.isBalanced();
    if (this.isOkChip()) {
      this.FilterString();
    }
  }
  onChange(e, id: number) {
    this.fruits[id].value = e.target.value;
    this.isRightOperator();
    this.isBalanced();

    this.paginator.pageIndex = 0;
    if (this.isOkChip()) {
      this.FilterString();
    }
  }
  isNullAllChip( ): boolean {
    this.fruits.forEach(function (value) {
      if (value.value === "") {
        if (!(value.name === '(' || value.name === ')' || value.name === 'И' || value.name === 'ИЛИ'))     
        return false;
      }
      
    });
   return true;
  }
  isNullChip(id: number): boolean{

    if (this.fruits[id].value === "") {
      if (this.fruits[id].name === '(' || this.fruits[id].name === ')' || this.fruits[id].name === 'И' || this.fruits[id].name === 'ИЛИ')
        return true;
      return false;
    }
    else return true;

  }
  selected(event: MatAutocompleteSelectedEvent): void {
    this.fruits.push({ name: event.option.viewValue, value: '', id: this.fruits.length });
    this.fruitInput.nativeElement.value = '';
    this.fruitCtrl.setValue(null);
    this.fruitInput.nativeElement.blur();
    this.isRightOperator();
    this.isBalanced();

    this.paginator.pageIndex = 0;
    if (this.isOkChip()) {
      this.FilterString();
    }

  }
  selectwarehouse(): void{
    this.FilterString();
  }
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.allFruits.filter(option => option.toLowerCase().includes(filterValue));
  }
  FilterString(): string {
    var str = this.GetFilterString();
    if (str != "---") {
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          startWith({}),
          switchMap(() => {
            this.isLoadingResults = true;
            return this.dataService.getUser(str, this.paginator.pageSize, this.sort.active, this.sort.direction, this.paginator.pageIndex, this.warehouse);
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
    this.prevFilter = str;
  return str;
  }

  GetFilterString(): string {
    var str = "";
    var i = 0;
    var copy = this.fruits;
    if (!this.isOkChip()) return "---";
    if (!this.isNullAllChip()) return "---";
    this.fruits.forEach(function (value) {
      if (value.name === 'Все поля') {
        if (value.value === '') str += "";
        else
        str += "(Номер_упаковки:\u0022" + value.value + "\u0022 OR Наименование_изделия: \u0022" + value.value + "\u0022 OR " + "Заводской_номер: \u0022" + value.value + "\u0022 OR " + "Обозначение: \u0022" + value.value + "\u0022 OR " + "Система: \u0022" + value.value + "\u0022 OR " + "Принадлежность: \u0022" + value.value + "\u0022 OR " + "Ответственный: \u0022" + value.value + "\u0022 OR " + "Местонахождение_на_складе: \u0022" + value.value + "\u0022 OR " + "Откуда: \u0022" + value.value + "\u0022 OR " + "Куда: \u0022" + value.value + "\u0022 OR " + "Примечание: \u0022" + value.value + "\u0022 OR " + "Добавил: \u0022" + value.value + "\u0022 )";
      }
      else {
        if (value.name === 'И') str += " AND ";
        else {
          if (value.name === 'ИЛИ') str += " OR ";
          else {
            if (value.name === '(') {
              if (i >= 1) {
                if (copy[i - 1].name != 'И' && copy[i - 1].name != 'ИЛИ' && copy[i - 1].name != '(')
                  str += " AND ";
              }
              str += " ( ";
            }
            else {
              if (value.value === '') str = "";
              else {
              if (value.name === ')') str += " ) ";
              else {
                if (i >= 1) {
                  if (copy[i - 1].name != 'И' && copy[i - 1].name != 'ИЛИ' && copy[i - 1].name != '(')
                    str += " AND ";
                }

                var name = value.name;
                if (name === 'Номер упаковки') name = 'Номер_упаковки';
                if (name === 'Наименование изделия') name = 'Наименование_изделия';
                if (name === 'Заводской номер') name = 'Заводской_номер';
                if (name === 'Местонахождение') name = 'Местонахождение_на_складе';
                str += name + ": \u0022" + value.value + "\u0022";
              }
            }
            }
          }
        }
      }
      i++;
    });
 
    return str;
  }
  isRightOperator(): boolean {
    var f = true;
    var copy = this.fruits;
    var i = 0;
    this.fruits.forEach(function (value) {
      
      if (i <= copy.length - 2) {
        if ((value.name === 'ИЛИ' && copy[i + 1].name === 'ИЛИ') || (value.name === 'И' && copy[i + 1].name === 'И') || (value.name === 'ИЛИ' && copy[i + 1].name === 'И') || (value.name === 'И' && copy[i + 1].name === 'ИЛИ') || (value.name === 'И' && copy[i + 1].name === '(') || (value.name === 'ИЛИ' && copy[i + 1].name === '(') || (value.name === '(' && copy[i + 1].name === ')'))
          f= false;
        }
     
      i++;
    });
    if (this.fruits.length > 0)
    {
      if (this.fruits[0].name === 'И' || this.fruits[0].name === 'ИЛИ') {
        f= false;
      }
      if (this.fruits[this.fruits.length - 1].name === 'И' || this.fruits[this.fruits.length - 1].name  === 'ИЛИ') {
     f =false;
      }
    }
    this.RightOperator = f;
    return f;
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
  isOkChip(): boolean {
    return this.Balanced && this.RightOperator;
  }
}
