export class NestExpansion {
    id: number;
    date: Date;
    frameType: FrameType;
    combSheets: number;
    combs: number;
    beefamilyId: number;
}

export enum FrameType {
    NestFrame, HoneyFrame
}

export const FrameType2LabelMapping: Record<FrameType, string> = {
    [FrameType.NestFrame]: "Dideli rėmai",
    [FrameType.HoneyFrame]: "Pusrėmiai"
};