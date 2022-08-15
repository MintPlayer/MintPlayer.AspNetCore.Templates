import { TwoFactorCodeUI } from "./two-factor-code-ui";

export interface TwoFactorCodeModal {
	isRequestingTwoFactorCode: boolean;
	twoFactorCode: TwoFactorCodeUI;
	allowRecoveryCode: boolean;
}
