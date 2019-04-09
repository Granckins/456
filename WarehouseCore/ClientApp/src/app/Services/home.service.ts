 
import { CouchRequest, EventCouch } from '../Models/home.model'
import { Injectable } from '@angular/core';
import {  Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';   
import { map } from 'rxjs/operators'; 
@Injectable()
export class DataSetService { 

  constructor(private http: HttpClient) { }

  getUser(sort: string, order: string, page: number): Observable<CouchRequest<EventCouch>> {
    const href = 'api/Data/FilterSortDocument';
    const requestUrl =
      `${href}?sort=${sort}&order=${order}&page=${page + 1}`;
    return this.http.get<CouchRequest<EventCouch>>(requestUrl);
  }

}
