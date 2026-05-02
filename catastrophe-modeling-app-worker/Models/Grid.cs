public class Grid
{
  public List<Cell> Cells { get; set; }

  public Grid()
  {
  }

  public Grid(List<Cell> cells)
  {
    Cells = cells;
  }

  public List<Cell> GetCells() => Cells;

}