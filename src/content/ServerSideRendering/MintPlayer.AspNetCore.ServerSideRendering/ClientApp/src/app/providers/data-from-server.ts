import { InjectionToken } from "@angular/core";
import { DataFromServer } from "../interfaces/data-from-server";

export const DATA_FROM_SERVER = new InjectionToken<DataFromServer>('DataFromServer');
