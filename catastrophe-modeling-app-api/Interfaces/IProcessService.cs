using Microsoft.AspNetCore.Mvc;

public interface IProcessService
{
  List<StormInfo> ProcessHURDAT2(IFormFile file);
}