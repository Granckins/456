import { Component, OnInit, InjectionToken } from '@angular/core'; 
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatProgressSpinnerModule  } from '@angular/material'; 
@Component({
  selector: 'export-data',
  templateUrl: 'export.component.html',
   
})

export class ExportComponent implements OnInit {
  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  checked = true;
  isLoading = true;
  disabled = true;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  constructor(private _formBuilder: FormBuilder) { }

  ngOnInit() {
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
  }
}
