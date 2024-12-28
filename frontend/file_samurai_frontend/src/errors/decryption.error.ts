export class DecryptionError extends Error {
    constructor(message: string = 'Error decrypting data. Please make sure password is correct. ') {
        super(message);
    }
}