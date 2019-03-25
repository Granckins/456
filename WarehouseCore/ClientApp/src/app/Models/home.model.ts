import { Data } from "@angular/router";

class RevsInfo {
  rev: string;
  status: string;
} 
 
export class EventCouch  {
  archive: boolean;
  _rev: string;
  _id: string;
  Nomer_upakovki: number;
  Naimenovanie_izdeliya: string;
  Zavodskoj_nomer: string;
  Kolichestvo: number;
  Oboznachenie: string;
  Sistema: string;
  Prinadlezhnost: string;
  Stoimost: number;
  Otvetstvenny: string;
  Mestonahozhdenie_na_sklade: string;
  Ves_brutto: number;
  Ves_netto: number;
  Dlina: number;
  Shirina: number;
  Vysota: number;
  Data_priyoma: Data;
  Otkuda: string;
  Data_vydachi: Data;
  Kuda: string;
  Nomer_plomby: string;
  Primechanie: string;
  Dobavil: string;
  Data_ismenen: Data;
  _revs_info: Array<RevsInfo> = new Array();

  constructor() {

  }
}


 export class RowCouch<Obj>
{
 id: string;
    key: string;
    page: string;

    filtername: string;
 filtervalue: string;
   sortname: string;
   sortvalue: string;
   datepr: string;
 datevd: string;

  limit: number;
  bool: boolean;
  value: Obj;
}
class CouchRequestClass<Obj>  implements CouchRequest <Obj> {
  total_rows: number;
  offset: number;
  rows: Array<RowCouch<Obj>> = new Array();
}

export interface CouchRequest<Obj>
{
  total_rows: number;
  offset: number;
  rows: Array<RowCouch<Obj>>;
  
    }
