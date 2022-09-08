import { Guid } from 'guid-typescript';

export interface User {
	id: Guid;
	userName: string;
	email: string;
//#if (UseTwoFactorAuthentication)

	isTwoFactorEnabled: boolean;
	bypass2faForExternalLogin: boolean;
//#endif
}
