 
import { Injectable } from '@angular/core'; 
import { HttpClient } from '@angular/common/http';
import { Http, ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs';
 
@Injectable()
export class ExportService {

  constructor(private http: HttpClient) { }
  download(): Observable<Blob> {
    const href = 'api/Data/ExportData';
    const requestUrl =
      `${href}`;
    return this.http.post<Blob>(requestUrl,  
      { responseType:   ResponseContentType.Blob  });
  }



}
