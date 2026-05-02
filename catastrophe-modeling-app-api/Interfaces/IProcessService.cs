using Microsoft.AspNetCore.Mvc;

public interface IProcessService
{
  List<StormObject> ProcessHURDAT2();
}