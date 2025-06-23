import { CheckupProcedure } from "./CheckupProcedure";

export interface Checkup{
    id: number;
    checkupTime: string;
    procedure: CheckupProcedure;
    patientId: number;
}