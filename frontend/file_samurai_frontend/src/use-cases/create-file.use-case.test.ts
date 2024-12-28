import {CreateFileUseCase} from './create-file.use-case';
import {FileService} from "../services/file.service";
import {KeyService} from "../services/key.service";
import {mock, mockReset} from 'jest-mock-extended'
import {CryptographyService} from "../services/cryptography.service";
import {EDITOR_ROLE} from "../constants";
import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";

describe('CreateFileUseCase', () => {
    const mockFileService = mock<FileService>();
    const mockKeyService = mock<KeyService>();
    const mockCryptoService = mock<CryptographyService>();

    const file = Buffer.from('file');
    const title = 'title';
    const groupId = 'groupId';
    const userId = 'userId';
    const userPublicKeyReturnValue = 'userPublicKey';

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

        mockCryptoService.encryptAes256Gcm.mockReturnValue(getAesEncryptionOutput());
        mockCryptoService.encryptWithPublicKey.mockReturnValue(getBuffer());
        mockCryptoService.generateKey.mockReturnValue(getBuffer());

        mockFileService.postFile.mockResolvedValue(GetAddFileResponseDto());
        mockFileService.convertToUserFileAccessDto.mockReturnValue(GetAddOrGetUserFileAccessDto());

        mockKeyService.getUserPublicKey.mockResolvedValue(userPublicKeyReturnValue);
    });

    describe('it should call services with correct values', () => {
        it('should call cryptoService encryptAes256Gcm with correct parameters', async () => {
            const spy = jest.spyOn(mockCryptoService, 'encryptAes256Gcm');
            const keyReturnValue = getBuffer();

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(file, keyReturnValue);
        });

        it('should call fileService postFile with correct parameters', async () => {
            const spy = jest.spyOn(mockFileService, 'postFile');
            const encryptionReturnValue = getAesEncryptionOutput();

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
            const keyReturnValue = getBuffer();

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(keyReturnValue, userPublicKeyReturnValue);
        });

        it('should call fileService convertToUserFileAccessDto with correct parameters', async () => {
            const spy = jest.spyOn(mockFileService, 'convertToUserFileAccessDto');
            const keyReturnValue = getBuffer();
            const dto = GetAddFileResponseDto();

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(keyReturnValue, userId, dto.id, EDITOR_ROLE);
        });

        it('should call fileService postUserFileAccess with correct parameters', async () => {
            const spy = jest.spyOn(mockFileService, 'postUserFileAccess');
            const dto = GetAddOrGetUserFileAccessDto();

            await useCase.execute(userId, groupId, file, title);

            expect(spy).toHaveBeenCalledTimes(1);
            expect(spy).toHaveBeenCalledWith(dto);
        });

        it('should return expected value', async () => {
            const result = await useCase.execute(userId, groupId, file, title);
            const dto = GetAddFileResponseDto();

            expect(result).toEqual(dto);
        });
    });
});

const getAesEncryptionOutput = (): AesGcmEncryptionOutput => ({
    cipherText: 'cipher',
    nonce: 'nonce',
    tag: 'tag',
})

const getBuffer = (): Buffer => Buffer.from('key');

const GetAddOrGetUserFileAccessDto = (): AddOrGetUserFileAccessDto => ({
    encryptedFileKey: 'FAK',
    role: EDITOR_ROLE,
    userId: 'userId',
    fileId: 'id',
});

const GetAddFileResponseDto = (): AddFileResponseDto => ({
    id: 'id',
    title: 'title',
})