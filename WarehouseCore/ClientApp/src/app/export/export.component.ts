import { Component, OnInit, InjectionToken } from '@angular/core'; 
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { saveAs   } from "file-saver";
import { ExportService } from '../Services/export.service';
import { MatStepper } from '@angular/material/stepper';
@Component({
  selector: 'export-data',
  templateUrl: 'export.component.html',
   
})

export class ExportComponent implements OnInit {
  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  checked = true;
  loading = false;
  disabled = true;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
 
  goBack(stepper: MatStepper) {
    stepper.previous();
  }

  goForward(stepper: MatStepper) {
    stepper.next();
  }
  constructor(private _formBuilder: FormBuilder, public dataExtractService: ExportService) { }
  createView() {

    // file-downloader.component.ts
    this.dataExtractService

      .download()
      .subscribe(response => {
        let data = JSON.stringify(response);
        let blob: any = new Blob([data], { type: 'application/json' }); 
        var today = new Date();
        var date = today.getFullYear() + '_' + (today.getMonth() + 1) + '_' + today.getDate();
        var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        var dateTime = date + ' ' + time;
        saveAs(blob, dateTime+'_data.json');
      }), error => console.log('Error downloading the file'),
      () => console.info('File downloaded successfully');
  
  }
  ngOnInit() {
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
  }
}
