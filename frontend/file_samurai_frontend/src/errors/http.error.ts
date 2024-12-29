export class HttpError extends Error {
    statusCode: number | undefined;
    data: unknown | undefined;
    constructor(message: string, statusCode: number | undefined, data?: unknown | undefined) {
        super(message);
        this.statusCode = statusCode;
        this.data = data
    }
}