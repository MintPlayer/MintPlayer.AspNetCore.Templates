import { ECodeType } from "../api/enums/code-type";

export interface TwoFactorCodeUI {
	verificationCode: string;
	recoveryCode: string;
	type: ECodeType;
}
