 
import { CouchRequest, EventCouch } from '../Models/home.model'
import { Injectable } from '@angular/core';
import {  Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';   
import { map } from 'rxjs/operators'; 
@Injectable()
export class DataSetService { 

  constructor(private http: HttpClient) { }

  getUser(): Observable<CouchRequest<EventCouch>> {
    return this.http.get<CouchRequest<EventCouch>>('api/Data/FilterSortDocument');
  }

}
