export class HttpError extends Error {
    statusCode: number | undefined;
    data: unknown;
    constructor(message: string, statusCode: number | undefined, data: unknown) {
        super(message);
        this.statusCode = statusCode;
        this.data = data
    }
}