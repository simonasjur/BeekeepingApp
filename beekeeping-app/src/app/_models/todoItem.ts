export class TodoItem {
    id: number;
    title: string;
    priority: TodoItemPriorities;
    description: string;
    dueDate: Date;
    isComplete: boolean;
    farmId: number;
    beehiveId: number;
    apiaryId: number;
    isDeleting: boolean = false;
}

export enum TodoItemPriorities {
    Low,
    Normal,
    High,
    Critical
}

export const TodoItemPriority2LabelMapping: Record<TodoItemPriorities, string> = {
    [TodoItemPriorities.Low]: "Žemas",
    [TodoItemPriorities.Normal]: "Normalus",
    [TodoItemPriorities.High]: "Aukštas",
    [TodoItemPriorities.Critical]: "Kritinis"
};
