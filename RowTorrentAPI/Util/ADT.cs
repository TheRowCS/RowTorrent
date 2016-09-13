using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowTorrentAPI.Util {
    public class ADT {
    }

    /// <summary>
    /// SuperPersistent™ storage. Guaranteed to never die.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VoldemortList<T> {
        public List<T> Diary;
        public List<T> Ring;
        public List<T> Locket;
        public List<T> HouseCup;
        public List<T> Diadem;
        public List<T> Harry;
        public List<T> Nagini;

        private VoldemortList<T> Avadakadavra;

        /// <summary>
        /// Creates a new VoldemortList object. Beware of it's power.
        /// </summary>
        /// <param name="horcruxes"></param>
        /// <param name="isReborn"></param>
        public VoldemortList(List<T> horcruxes, bool isReborn = false) {
            Diary = new List<T>(horcruxes);
            Ring = new List<T>(horcruxes);
            Locket = new List<T>(horcruxes);
            HouseCup = new List<T>(horcruxes);
            Diadem = new List<T>(horcruxes);
            Harry = new List<T>(horcruxes);
            Nagini = new List<T>(horcruxes);

            if (isReborn) {
                Avadakadavra = new VoldemortList<T>(horcruxes, isReborn = true);
            }
        }
    }
}