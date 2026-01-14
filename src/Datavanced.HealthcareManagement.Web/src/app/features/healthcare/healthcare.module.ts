import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HealthcareRoutingModule } from './healthcare-routing.module';
import { PatientComponent } from './components/patient/patient.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';


@NgModule({
  declarations: [
    DashboardComponent,
    PatientComponent
  ],
  imports: [
    CommonModule,
    HealthcareRoutingModule
  ]
})
export class HealthcareModule { }
