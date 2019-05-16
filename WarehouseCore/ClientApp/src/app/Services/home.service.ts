 
import { CouchRequest, EventCouch } from '../Models/home.model'
import { Injectable } from '@angular/core';
import {  Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';   
import { map } from 'rxjs/operators'; 
@Injectable()
export class DataSetService { 

  constructor(private http: HttpClient) { }

  getUser(filter: string, pagesize: number, sort: string, order: string, page: number, warehouse: string): Observable<CouchRequest<EventCouch>> {
    const href = 'api/Data/FilterSortDocument';
    const requestUrl =
      `${href}?filter=${filter}&pagesize=${pagesize}&sort=${sort}&order=${order}&page=${page + 1}&warehouse=${warehouse}`;
    return this.http.get<CouchRequest<EventCouch>>(requestUrl);
  }

}
