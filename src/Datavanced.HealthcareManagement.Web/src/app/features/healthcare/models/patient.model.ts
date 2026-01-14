import { Caregiver } from "./caregiver.model";

export interface Patient {
  patientId: number;
  firstName: string;
  lastName: string;
  officeId: number;
  officeName: string;
  dateOfBirth: string;
  phone: string;
  email: string;
  isActive: boolean;
  caregivers: Caregiver[];
}

export interface CreatePatientDto {
  officeId: number;
  firstName: string;
  lastName: string;
  dateOfBirth?: string | Date; 
  phone?: string;
  email?: string;
  isActive: boolean;
  caregivers: number[]; 
}

