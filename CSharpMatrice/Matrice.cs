using System.Reflection;
using System.ComponentModel;
using System.IO;
using System.Reflection.Emit;
using System.Linq;
using System;

namespace CSharpMatrice;
public class Matrice
{
    #region variable d'instance
    private List<List<double>> _composantes = new List<List<double>>();
    #endregion 

    #region constructeur
    public Matrice(int lignes, int colonnes, double composante)
    {
        if (IsOutOfBound(lignes, colonnes)) throw new Exception();

        _composantes = Enumerable.Range(0, lignes)
            .Select(ligne => Enumerable.Range(0, colonnes)
            .Select(element => composante)
            .ToList())
            .ToList();
    }
    
    public Matrice(int lignes, int colonnes, params double[] composantes)
    {
        List<double> tempComposantes;
        int start = 0;
        int end = colonnes;
        Range range;

        if (lignes < 1 || colonnes < 1)
        {
            throw new Exception();
        }

        if (lignes * colonnes != composantes.Length)
        {
            throw new Exception();
        }

        for (int iLigne = 0; iLigne < lignes; iLigne++)
        {
            range = new Range(start, end);
            tempComposantes = new List<double>();
            tempComposantes.AddRange(composantes[range]);

            start += colonnes;
            end += colonnes;

            _composantes.Add(tempComposantes);
        }
    }

    public Matrice(IEnumerable<IEnumerable<double>> composantes)
    {
        _composantes = composantes.Select(element => element.ToList()).ToList();
    }

    public Matrice(Matrice matrice)
    {
        for (int iLigne = 0; iLigne < matrice.Lignes; iLigne++)
        {
            _composantes.Add(new List<double>());

            for (int iColonne = 0; iColonne < matrice.Colonnes; iColonne++)
            {
                _composantes[iLigne].Add(matrice.GetComposante(iLigne, iColonne)); 
            }
        }
    }

    #endregion

    #region getter
    public double GetComposante(int ligne, int colonne)
    {
        if (ligne < 0 || colonne < 0 || ligne >= this.Lignes || colonne >= this.Colonnes)
            throw new Exception();

        return _composantes.ElementAt(ligne).ElementAt(colonne);
    }

    public int Lignes
    {
        get => _composantes.ToList().Count;
    }

    public int Colonnes
    {
        get => _composantes.ElementAt(0).ToList().Count;
    }
    #endregion

    public bool EstCarree 
    {
        get => this.Lignes == this.Colonnes;
    }

    public void SetComposante(int ligne, int colonne, double reel) 
    {
        if(IsOutOfBound(ligne, colonne)) throw new Exception();
        if (ligne >= this.Lignes || colonne >= this.Colonnes) throw new Exception();

        _composantes[ligne][colonne] = reel;
    }

    private static bool IsOutOfBound(int line, int column)
    {
        return IsOutOfBoundLine(line) || IsOutOfBoundColumn(column);
    }

    private static bool IsOutOfBoundLine(int line)
    {
        return line < 0;
    }

    private static bool IsOutOfBoundColumn(int column)
    {
        return column < 0;
    }

    public void MultiplicationInterne(double reel)
    {
        _composantes = _composantes.Select(ligne => ligne.Select(element => element * reel).ToList()).ToList();
    }

    public void DivisionInterne(double reel)
    {
        MultiplicationInterne(1/reel);
    }

    public void MultiplicationExterne(Matrice matrice)
    {
        //TODO écrire la methode
    }

    // utilisation d'une déléguée 
    private delegate double Calculation(double a, double b);

    public void SoustractionMatricielle(Matrice matrice)
    {
        ParcoursSimple(matrice, (a, b) => a - b);
    }

    public void AdditionMatricielle(Matrice matrice)
    {
        ParcoursSimple(matrice, (a, b) => a + b);
    }

    private void ParcoursSimple(Matrice matrice, Calculation action)
    {
        if (Colonnes != matrice.Colonnes || Lignes != matrice.Lignes)
            throw new Exception();

        _composantes = _composantes.Select((ligne, iLigne) => ligne
            .Select((element, iColonne) => action(element, matrice.GetComposante(iLigne, iColonne)))
            .ToList())
            .ToList();
    }

    public static Matrice Transposer(Matrice matrice)
    {
        var newMatrice = new List<List<double>>(matrice.Colonnes);
        
        for (int iLigne = 0; iLigne < matrice.Lignes; iLigne++)
        {
            newMatrice.Add(new List<double>());

            for (int iColonne = 0; iColonne < matrice.Colonnes; iColonne++)
            {
                newMatrice[iColonne].Add(matrice.GetComposante(iLigne, iColonne));
            }
        }
        return new Matrice(newMatrice);
    }

    public void MultiplicationLigne(int ligne, double reel)
    {
        _composantes[ligne] = _composantes[ligne].Select(composante => composante * reel).ToList();
    }

    public void DivisionLigne(int ligne, double reel)
    {
        MultiplicationLigne(ligne, 1/reel);
    }

    public void MultiplicationColonne(int colonne, double reel)
    {
        _composantes.ForEach(ligne => ligne[colonne] *= reel);
    }

    public void DivisionColonne(int colonne, double reel) 
    {
        MultiplicationColonne(colonne, 1/reel);
    }

    public void PermutationLigne(int ligne1, int ligne2)
    {
        (_composantes[ligne1], _composantes[ligne2]) = (_composantes[ligne2], _composantes[ligne1]);
    }

    public void PermutationColonne(int colonne1, int colonne2)
    {
        _composantes.ForEach(ligne => (ligne[colonne1], ligne[colonne2]) = (ligne[colonne2], ligne[colonne1]));
    }

    public void Exposant(int exposant)
    {
        _composantes = _composantes.Select(ligne => ligne.Select(element => Math.Pow(element, exposant)).ToList()).ToList();
    }

    public void AdditionLignes(int ligne1, double multiple, int ligne2)
    {
        _composantes[ligne1] = _composantes.ElementAt(ligne2)
            .Select((element, index) => _composantes.ElementAt(ligne1).ElementAt(index) + element * multiple)
            .ToList();
    }

    public void SoustractionLignes(int ligne1, double multiple, int ligne2)
    {
        _composantes[ligne1] = _composantes.ElementAt(ligne2)
            .Select((element, index) => _composantes.ElementAt(ligne1).ElementAt(index) - element * multiple)
            .ToList();
    }

    public override string ToString() 
    {
        return _composantes
            .Select(ligne => ligne
            .Select(element => $"|{element.ToString()}")
            .Aggregate((string1, string2) => string1 + string2))
            .Aggregate((string1, string2) => string1 + string2) + $"|{Environment.NewLine}";
    }
}
