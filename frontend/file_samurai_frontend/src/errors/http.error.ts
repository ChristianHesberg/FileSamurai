export class HttpError extends Error {
    statusCode: number | undefined;
    constructor(message: string, statusCode: number | undefined) {
        super(message);
        this.statusCode = statusCode;
    }
}