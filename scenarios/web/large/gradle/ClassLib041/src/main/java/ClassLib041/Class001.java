package ClassLib041;

public class Class001 {
    public static String property() {
        return "ClassLib041" + " " + ClassLib007.Class001.property() + " " + ClassLib022.Class001.property() + " " + ClassLib011.Class001.property() + " " + ClassLib026.Class001.property();
    }
}
