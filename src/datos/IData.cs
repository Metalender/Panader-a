namespace datos;
using System.Collections.Generic;

public interface IData<T>
{
    public void guardar(List<T> datos);
    public List<T> leer();

}