export interface ChangeAdminPasswordModal {
	isChangingPassword: boolean;
	bearerToken: string | null;
	newPassword: string;
	newPasswordConfirmation: string;
}
