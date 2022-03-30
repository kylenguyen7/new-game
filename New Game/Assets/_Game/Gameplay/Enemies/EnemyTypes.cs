

using System;

public static class EnemyTypes {
    public static String EnemyTypeToString(EnemyType type) {
        switch (type) {
            case EnemyType.DASHER:
                return "Dasher";
            case EnemyType.SHOOTER:
                return "Shooter";
            case EnemyType.MUSHROOM:
                return "Mushroom";
            case EnemyType.CHERRY:
                return "Cherry";
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
    
public enum EnemyType {
    DASHER,
    SHOOTER,
    MUSHROOM,
    CHERRY
}