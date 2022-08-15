import { ECodeType } from "../enums/code-type";

export interface TwoFactorCode {
	code: string;
	type: ECodeType;
}
