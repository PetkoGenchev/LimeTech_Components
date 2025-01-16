import { PartStatus } from "./status.dto";

export interface ComponentDTO {

  id: number;
  name: string | null;
  typeOfProduct: string | null;
  imageUrl: string | null;
  price: number;
  purchasedCount: number;
  productionYear: number;
  powerUsage: number;
  status: PartStatus;
  stockCount: number;
  isPublic: boolean;

}
