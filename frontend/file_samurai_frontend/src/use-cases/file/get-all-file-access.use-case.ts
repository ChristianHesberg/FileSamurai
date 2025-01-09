import {FileService} from "../../services/file.service";

export class GetAllFileAccessUseCase {
    constructor(private readonly fileService: FileService) {
    }

    async execute(fileId: string) {
        return await this.fileService.getAllFileAccess(fileId)
    }

}