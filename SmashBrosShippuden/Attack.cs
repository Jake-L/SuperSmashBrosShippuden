namespace SmashBrosShippuden
{
    internal class Attack
    {
        private readonly string character;
        public readonly AttackType attackType;
        public int spriteLength;
        public int[] attackFrame;
        public int damage;
        public int knockback;
        public bool createProjectile = false;

        public Attack(string character, AttackType attackType)
        {
            this.character = character;
            this.attackType = attackType;
            this.Initialize();
        }

        private void Initialize()
        {
            if (character == "Mario")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 10;
                    this.knockback = 2;
                    this.spriteLength = 8;
                    this.attackFrame = new int[] { 5 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 5;
                    this.createProjectile = true; 
                    this.attackFrame = new int[] { 4 };
                }
            }
            if (character == "Luigi")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 10;
                    this.knockback = 2;
                    this.spriteLength = 7;
                    this.attackFrame = new int[] { 5 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 5;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 4 };
                }
            }
            if (character == "Mewtwo")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 13;
                    this.knockback = 2;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 4;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 3 };
                }
            }
            if (character == "Sonic")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 9;
                    this.knockback = 2;
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 14;
                    this.knockback = 2;
                    this.spriteLength = 9;
                    this.attackFrame = new int[] { 4 };
                }
            }
            if (character == "Shadow")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 13;
                    this.knockback = 2;
                    this.spriteLength = 9;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 2;
                    this.knockback = 1;
                    this.spriteLength = 17;
                    this.attackFrame = new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };
                }
            }
            if (character == "Knuckles")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 9;
                    this.knockback = 2;
                    this.spriteLength = 6;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 13;
                    this.knockback = 1;
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 2 };
                }
            }
            if (character == "Blastoise")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 8;
                    this.knockback = 2;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 1 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 3;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 0, 1, 2, 3 };
                }
            }
            if (character == "Pichu")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 8;
                    this.knockback = 1;
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 2 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 5;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 1 };
                }
            }
            if (character == "Charizard")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 10;
                    this.knockback = 2;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 4;
                    this.createProjectile = true;
                    this.attackFrame = new int[] { 2 };
                }
            }
            if (character == "Kirby")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 7;
                    this.knockback = 1;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 9;
                    this.knockback = 2;
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 3 };
                }
            }
            if (character == "Metaknight")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 9;
                    this.knockback = 1;
                    this.spriteLength = 9;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 13;
                    this.knockback = 2;
                    this.spriteLength = 8;
                    this.attackFrame = new int[] { 4 };
                }
            }
            if (character == "King")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 15;
                    this.knockback = 2;
                    this.spriteLength = 6;
                    this.attackFrame = new int[] { 5 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.spriteLength = 5;
                    this.attackFrame = new int[] { 3 };
                }
            }
            if (character == "Link")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 8;
                    this.knockback = 1;
                    this.spriteLength = 7;
                    this.attackFrame = new int[] { 4 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 9;
                    this.knockback = 2;
                    this.spriteLength = 8;
                    this.attackFrame = new int[] { 3, 7 };
                }
            }

            if (character == "Shrek")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 10;
                    this.knockback = 1;
                    this.spriteLength = 6;
                    this.attackFrame = new int[] { 3 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 15;
                    this.knockback = 2;
                    this.spriteLength = 8;
                    this.attackFrame = new int[] { 4 };
                }
            }

            if (character == "Sasuke")
            {
                if (this.attackType == AttackType.Jab)
                {
                    this.damage = 6;
                    this.knockback = 1;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 2 };
                }
                else if (this.attackType == AttackType.Special)
                {
                    this.damage = 10;
                    this.knockback = 2;
                    this.spriteLength = 4;
                    this.attackFrame = new int[] { 2 };
                }
            }
        }

        public void update()
        {

        }
    }
}
