import {CreateFileUseCase} from './create-file.use-case';
import {FileService} from "../services/file.service";
import {KeyService} from "../services/key.service";
import {mock, mockReset} from 'jest-mock-extended'
import {CryptographyService} from "../services/cryptography.service";
import {EDITOR_ROLE} from "../constants";

describe('CreateFileUseCase', () => {
    const mockFileService = mock<FileService>();
    const mockKeyService = mock<KeyService>();
    const mockCryptoService = mock<CryptographyService>();

    const file = Buffer.from('file');
    const title = 'title';
    const groupId = 'groupId';
    const userId = 'userId';

    let useCase: CreateFileUseCase;

    beforeEach(async () => {
        mockReset(mockFileService);
        mockReset(mockKeyService);
        mockReset(mockCryptoService);

        useCase = new CreateFileUseCase(
            mockFileService,
            mockKeyService,
            mockCryptoService
        );

        mockCryptoService.encryptAes256Gcm.mockReturnValue(encryptionReturnValue);

        mockCryptoService.encryptWithPublicKey.mockReturnValue(encryptWithPublicKeyReturnValue);
        mockCryptoService.generateKey.mockReturnValue(generateKeyReturnValue);

        mockFileService.postFile.mockResolvedValue(postFileReturnValue);

        mockFileService.convertToUserFileAccessDto.mockReturnValue(convertToDtoReturnValue);

        mockKeyService.getUserPublicKey.mockResolvedValue(userPublicKeyReturnValue);
    });

    describe('it should call services with correct values', () => {
        it('should call cryptoService encryptAes256Gcm with correct parameters', async () => {
            const spy = jest.spyOn(mockCryptoService, 'encryptAes256Gcm');

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(file, generateKeyReturnValue);
        });

        it('should call fileService postFile with correct parameters', async () => {
            const spy = jest.spyOn(mockFileService, 'postFile');
            const expectedCallValue = {
                fileContents: encryptionReturnValue.cipherText,
                nonce: encryptionReturnValue.nonce,
                tag: encryptionReturnValue.tag,
                title: title,
                groupId: groupId
            }

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(expectedCallValue);
        });

        it('should call keyService getUserPublicKey with correct parameters', async () => {
            const spy = jest.spyOn(mockKeyService, 'getUserPublicKey');

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(userId);
        });

        it('should call cryptoService encryptWithPublicKey with correct parameters', async () => {
            const spy = jest.spyOn(mockCryptoService, 'encryptWithPublicKey');

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(generateKeyReturnValue, userPublicKeyReturnValue);
        });

        it('should call fileService convertToUserFileAccessDto with correct parameters', async () => {
            const spy = jest.spyOn(mockFileService, 'convertToUserFileAccessDto');

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(encryptWithPublicKeyReturnValue, userId, postFileReturnValue.id, EDITOR_ROLE);
        });

        it('should call fileService postUserFileAccess with correct parameters', async () => {
            const spy = jest.spyOn(mockFileService, 'postUserFileAccess');

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(convertToDtoReturnValue);
        });

        it('should return expected value', async () => {
            const result = await useCase.execute(userId, groupId, file, title);

            expect(result).toEqual(postFileReturnValue);
        });
    });
});

const encryptionReturnValue = {
    cipherText: 'cipher',
    nonce: 'nonce',
    tag: 'tag',
}
const generateKeyReturnValue = Buffer.from('key');
const encryptWithPublicKeyReturnValue = Buffer.from('FAK');
const userPublicKeyReturnValue = 'userPublicKey';
const convertToDtoReturnValue = {
    encryptedFileKey: 'FAK',
    role: EDITOR_ROLE,
    userId: 'userId',
    fileId: 'id',
};
const postFileReturnValue = {
    id: 'id',
    title: 'title',
}