import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HealthcareRoutingModule } from './healthcare-routing.module';
import { PatientComponent } from './components/patient/patient.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { OfficeComponent } from './components/office/office.component';
import { CaregiverComponent } from './components/caregiver/caregiver.component';


@NgModule({
  declarations: [
    DashboardComponent,
    PatientComponent,
    OfficeComponent,
    CaregiverComponent
  ],
  imports: [
    CommonModule,
    HealthcareRoutingModule
  ]
})
export class HealthcareModule { }
