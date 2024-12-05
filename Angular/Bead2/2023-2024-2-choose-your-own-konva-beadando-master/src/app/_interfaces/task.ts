export interface Task {
  imageUrl: string;
  preview?: { imageUrl: string; text: string };
  story: string;
  subTasks: { text: string; xp : number }[];
  title: string;
}
