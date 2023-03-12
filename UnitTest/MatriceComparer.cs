namespace UnitTest
{
    public class MatriceComparer : IEqualityComparer<Matrice?>
    {
        public bool Equals(Matrice? m1, Matrice? m2)
        {
            if (m1 == null || m2 == null)
                return false;
            if (m1.Lignes != m2.Lignes || m1.Colonnes != m2.Colonnes)
                return false;

            for (int iLigne = 0; iLigne < m1.Lignes; iLigne++)
            {
                for (int iColonne = 0; iColonne < m1.Colonnes; iColonne++)
                {
                    if (m1.GetComposante(iLigne, iColonne) != m2.GetComposante(iLigne, iColonne))
                        return false;
                }
            }

            return true;
        }

        public int GetHashCode(Matrice m)
        {
            return m.ToString().GetHashCode();
        }
    }
}